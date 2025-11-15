const Kinect2 = require('kinect2');
const WebSocket = require('ws');

const kinect = new Kinect2();
const wss = new WebSocket.Server({ port: 8080 });

if(kinect.open()) {
    console.log("Kinect Opened");

    kinect.openBodyReader();

    // Bağlanan tüm WebSocket clientlarına veri gönderecek
    wss.on('connection', function connection(ws) {
        console.log('Unity connected via WebSocket');
    });

    kinect.on('bodyFrame', (bodyFrame) => {
        bodyFrame.bodies.forEach(body => {
            if(body.tracked) {
                // Örnek: sağ ve sol el koordinatlarını JSON olarak gönder
                const jointsToSend = ['HandRight', 'HandLeft', 'Head', 'SpineMid'];

                jointsToSend.forEach(jointName => {
                    const joint = body.joints[Kinect2.JointType[jointName]];
                    if(joint) {
                        const data = {
                            joint: jointName,
                            x: joint.cameraX,
                            y: joint.cameraY,
                            z: joint.cameraZ
                        };
                        // Tüm bağlı clientlara gönder
                        wss.clients.forEach(client => {
                            if(client.readyState === WebSocket.OPEN) {
                                client.send(JSON.stringify(data));
                            }
                        });
                    }
                });
            }
        });
    });
} else {
    console.log("Kinect not found!");
}
