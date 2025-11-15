const Kinect2 = require('kinect2');
const kinect = new Kinect2();

if (kinect.open()) {
  console.log("Kinect is open");

  // sadece bodyFrame açıyoruz
  kinect.openBodyReader();

  kinect.on('bodyFrame', (bodyFrame) => {
    bodyFrame.bodies.forEach((body, index) => {
      if (body.tracked) {
        const leftHand = body.joints[Kinect2.JointType.handLeft];
        const rightHand = body.joints[Kinect2.JointType.handRight];

        console.log(`Body ${index}:`);
        console.log(`  Left Hand -> x: ${leftHand.cameraX.toFixed(2)}, y: ${leftHand.cameraY.toFixed(2)}, z: ${leftHand.cameraZ.toFixed(2)}`);
        console.log(`  Right Hand -> x: ${rightHand.cameraX.toFixed(2)}, y: ${rightHand.cameraY.toFixed(2)}, z: ${rightHand.cameraZ.toFixed(2)}`);
      }
    });
  });
}
