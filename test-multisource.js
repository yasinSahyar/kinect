const Kinect2 = require(".");
const kinect = new Kinect2();

if (kinect.open()) {
  console.log("Kinect Opened");

  // Renk + Infrared gibi birden fazla veri kaynağını açıyoruz
  kinect.openMultiSourceReader({
    frameTypes: Kinect2.FrameType.color | Kinect2.FrameType.longExposureInfrared | Kinect2.FrameType.depth | Kinect2.FrameType.body
  });

  console.log("Started multi source reader");

  kinect.on("multiSourceFrame", (frame) => {
    const hasColorInfo = (frame.color && frame.color.buffer);
    const hasDepthInfo = (frame.depth && frame.depth.buffer);
    const hasInfraredInfo = (frame.infrared && frame.infrared.buffer);
    const hasLongExposureInfraredInfo = (frame.longExposureInfrared && frame.longExposureInfrared.buffer);
    const hasBodyInfo = (frame.body);

    console.log(
      `color: ${!!hasColorInfo}, depth: ${!!hasDepthInfo}, body: ${!!hasBodyInfo}, infrared: ${!!hasInfraredInfo}, longExposureInfrared: ${!!hasLongExposureInfraredInfo}`
    );

    // Eğer body datası varsa, iskelet noktalarını da yazalım
    if (hasBodyInfo && frame.body.bodies) {
      frame.body.bodies.forEach((body, idx) => {
        if (body.tracked) {
          const rightHand = body.joints[11]; // 11 = sağ el
          console.log(
            `Body ${idx} Right Hand → X:${rightHand.cameraX.toFixed(2)} Y:${rightHand.cameraY.toFixed(2)} Z:${rightHand.cameraZ.toFixed(2)}`
          );
        }
      });
    }
  });
} else {
  console.error("Kinect not detected!");
}
