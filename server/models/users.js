import { Heap } from 'heap-js';
import { MongoClient } from "mongodb";
import functions from "./tokens.js"

const key = "my secret key";


async function insrtUser(details) {
    const client = new MongoClient("mongodb://127.0.0.1:27017");
    try {
        client.connect();
        const db = client.db('CodeCraze');
        const statistics = db.collection('Statistics');
        const users = db.collection('Users');
        let res = await users.find({ username: details.username }).toArray();
        if (res.length > 0) {
            return 409;
        } else {
            const ret = { username: details.username, password: details.password, mail: details.mail, age: details.age, world: "Forest", task: "Swing", state: 0, score: 0 };
            await users.insertOne(ret);
            statistics.insertOne({username: details.username, notAnswered: 0, IO: [0, 0], Vars: [0, 0], Arithmetic: [0, 0], Logic: [0, 0], If: [0, 0], Loops: [0, 0], Arrays: [0, 0], Functions: [0, 0], Class: [0, 0], Recursion: [0, 0]})
            return ret;
        }
    } finally {
        client.close();
    }
}

async function getUserInfo(bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        let res = await users.find({ username: data.username }).toArray();
        return res[0];
    } catch (err) {
        return 401;
    } finally {
    }
}

async function delUser(bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        const statistics = db.collection('Statistics');
        let res = await users.deleteOne({ username: data.username });
        await statistics.deleteOne({ username: data.username });
        return 200;
    } catch (err) {
        return 401;
    } finally {
    }
}

async function updateScore(bearer, token, score) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        await users.updateOne({ username: data.username }, { $inc: { score: parseInt(score) } });
        return 200;
    } catch (err) {
        return 401;
    } finally {
    }
}

async function getTopScore(bearer, token, score) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        let res = await users.find({} , {username: 1, password: 0, mail: 0, age: 0, world: 0, task: 0, state: 0, score: 1 }).toArray();
        const myHeap = new Heap((user1, user2) => {
            if (user2.score === user1.score) {
                return user1.username.localeCompare(user2.username);
            }
            return user1.score - user2.score;
        });
        res.forEach(user => {
            myHeap.push(user);
            if (myHeap.size() > 5) {
                myHeap.pop();   
            }
        });
        return myHeap.toArray().sort((user1, user2) => {
            if (user2.score === user1.score) {
                return user1.username.localeCompare(user2.username);
            }
            return user2.score - user1.score;
        });
    } catch (err) {
        return 401;
    } finally {
    }
}

async function resetUserModels(bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        await users.updateOne({ username: data.username },
            { $set: { world: "Forest", task: "Swing", state: 0, score: 0 } });
        return 200;
    } catch (err) {
        return 401;
    } finally {
    }
}

async function saveState(bearer, token, details) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        await users.updateOne({ username: data.username },
            { $set: { world: details.world, task: details.task, state: parseInt(details.state) } });
        return 200;
    } catch (err) {
        return 401;
    } finally {
    }
}

async function getState(bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        let res = await users.find({ username: data.username }).toArray();
        if (res.length == 0) {
            return 400;
        }
        return { world: res[0].world, task: res[0].task, state: res[0].state };

    } catch (err) {
        return 401;
    } finally {
    }
}

export default { insrtUser, getUserInfo, delUser, resetUserModels, updateScore, getTopScore, saveState, getState }