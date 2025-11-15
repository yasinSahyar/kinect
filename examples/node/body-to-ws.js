const Kinect2 = require('kinect2');
const kinect = new Kinect2();

const WebSocket = require('ws');
const wss = new WebSocket.Server({ port: 8080 });

function broadcast(data) {
  const msg = JSON.stringify(data);
  wss.clients.forEach(client => {
    if (client.readyState === WebSocket.OPEN) {
      client.send(msg);
    }
  });
}

console.log("WebSocket started on ws://localhost:8080");

if (kinect.open()) {
  console.log("Kinect opened");

  kinect.on('bodyFrame', (bodyFrame) => {
    broadcast(bodyFrame);
  });

  kinect.openBodyReader();
}
