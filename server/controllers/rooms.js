import myModels from '../models/rooms.js'

const returnAllRooms = async (req, res) => {
    const myRes = await myModels.returnAllRoomsModels(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1], req.params.topics);
    if (myRes != 401) {
        res.status(200).send(myRes);
    } else {
        res.status(400);
    }
    res.end();
}

const createRoom = async (req, res) => {
    const myRes = await myModels.createRoomModels(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1], req.params.username, req.body);
    res.status(myRes);
    res.end();
}

const removeRoom = async (req, res) => {
    const myRes = await myModels.removeRoomModels(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1], req.params.username);
    res.status(myRes);
    res.end();
}

const joinRoom = async (req, res) => {
    const myRes = await myModels.joinRoomModels(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1], req.params.username, req.body);
    res.status(myRes);
    res.end();
}

function getKey(myMap, myValue) {
    for(let [key, value] of myMap) {
        if (myValue === value) {
            return key;
        }
    }
    return -1;
}

const startGame = async (req, res) => {
    const myRes = await myModels.startGameModels(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1], req.params.username);
    res.status(myRes);
    res.end();
}

export { returnAllRooms, createRoom, removeRoom, joinRoom, startGame, getKey};