//import myModels from '../models/rooms.js'

const roomsList = new Map();

const returnAllRooms = async (req, res) => {
    //const myRes = await myModels.getRooms(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    let myRes = {host : []}
    for (let [key, value] of roomsList) {
        if (value === "--"){
            myRes.host.push(key);
        }
    }
    if (myRes !== null) {
        res.status(200).send(myRes);
    } else {
        res.status(400);
    }
    res.end();
}

const createRoom = async (req, res) => {
    //const myRes = await myModels.postRooms(req.params.username, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    roomsList.set(req.params.username, "--");
    console.log(roomsList);
    res.status(200);
    res.end();
}

const removeRoom = async (req, res) => {
    //const myRes = await myModels.deleteRooms(req.params.username ,req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    roomsList.delete(req.params.username);
    res.status(200);
    res.end();
}

const joinRoom = async (req, res) => {
    //const myRes = await myModels.deleteRooms(req.params.username ,req.body.username ,req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (roomsList.get(req.params.username) === "--") { //could be sync problem
        roomsList.set(req.params.username, req.body.player);
        console.log(roomsList);
        res.status(200);
    } else {
        res.status(400);
    }
    res.end();
}


export { returnAllRooms, createRoom, removeRoom, joinRoom };