import { arrSoc } from "../app.js"
import functions from "./tokens.js"

export const roomsList = new Map();
export const roomsListTopics = new Map();

const OBS_NUMBER = 45

async function returnAllRoomsModels(bearer, token, topics) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        let myRes = { host: [] }
        for (let [key, value] of roomsList) {
            if (value === "--" && (topics + ' ') === roomsListTopics.get(key)) {
                myRes.host.push(key);
            }
        }
        return myRes;
    } catch (err) {
        return 401;
    } finally {
    }
}

async function createRoomModels(bearer, token, username, details) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        roomsList.set(username, "--");
        roomsListTopics.set(username, details.topics)
        return 200;
    } catch (err) {
        return 401;
    } finally {
    }
}

async function removeRoomModels(bearer, token, username) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        if (roomsList.get() !== "--") {
            let player2 = roomsList.get(username);
            roomsList.delete(username);
            roomsListTopics.delete(username);
            arrSoc.get(player2).emit("removeRoom");
        } else {
            roomsList.delete(username);
            roomsListTopics.delete(username);
        }
        return 200;
    } catch (err) {
        return 401;
    } finally {
    }
}

async function joinRoomModels(bearer, token, username, details) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        if (roomsList.get(username) === "--") {
            roomsList.set(username, details.player);
            arrSoc.get(username).emit('join', details.player);
            return 200;
        } else {
            return 401;
        }
    } catch (err) {
        return 401;
    } finally {
    }
}

async function startGameModels(bearer, token, username) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        var objs = "";
        for (let i = 0; i < OBS_NUMBER; i++) {
            objs += Math.round(Math.random());
        }
        arrSoc.get(username).emit('obs', objs);
        let p2 = roomsList.get(username);
        arrSoc.get(p2).emit('obs', objs);
        return 200;
    } catch (err) {
        return 401;
    } finally {
    }
}

export default { returnAllRoomsModels, createRoomModels, removeRoomModels, joinRoomModels, startGameModels }