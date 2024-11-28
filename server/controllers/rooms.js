//import myModels from '../models/rooms.js'
import {myIo ,arrSoc} from "../app.js"
export const roomsList = new Map();
export const roomsListTopics = new Map();

const returnAllRooms = async (req, res) => {
    //const myRes = await myModels.getRooms(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    let myRes = {host : []}
    console.log("-" + req.params.topics + "-");
    for (let [key, value] of roomsList) {
        if (value === "--" && (req.params.topics + ' ') === roomsListTopics.get(key)){
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
    console.log(req.body.topics);
    //const myRes = await myModels.postRooms(req.params.username, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    roomsList.set(req.params.username, "--");
    roomsListTopics.set(req.params.username, req.body.topics)
    console.log(roomsList);
    console.log(roomsListTopics);
    res.status(200);
    res.end();
}

const removeRoom = async (req, res) => {
    //const myRes = await myModels.deleteRooms(req.params.username ,req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (roomsList.get(req.params.username) !== "--") {
        let player2 = roomsList.get(req.params.username);
        arrSoc.get(player2).emit("removeRoom");
        //arrSoc.delete(getKey(arrSoc, player2));
    }
    roomsList.delete(req.params.username);
    roomsListTopics.delete(req.params.username);
    //arrSoc.delete(getKey(req.params.username));
    res.status(200);
    res.end();
}

const joinRoom = async (req, res) => {
    //const myRes = await myModels.deleteRooms(req.params.username ,req.body.username ,req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (roomsList.get(req.params.username) === "--") { //could be sync problem
        roomsList.set(req.params.username, req.body.player);
        console.log(roomsList);
        arrSoc.get(req.params.username).emit('join', req.body.player);
        res.status(200);
    } else {
        res.status(400);
    }
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
    var objs = "";
    for (let i = 0; i < 45; i++) {
        objs += Math.round(Math.random());
    }
    //console.log(req.params.username + " VS " + roomsList.get(req.params.username))
    arrSoc.get(req.params.username).emit('obs', objs);
    let p2 = roomsList.get(req.params.username);
    //console.log(getKey(arrSoc, p2));
    arrSoc.get(p2).emit('obs', objs);
    res.status(200);
    res.end();
}

export { returnAllRooms, createRoom, removeRoom, joinRoom, startGame, getKey};