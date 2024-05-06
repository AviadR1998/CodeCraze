import { MongoClient } from "mongodb";
import jwt from "jsonwebtoken";

const key = "my secret key";
const smallBearer = "bearer";
const bigBearer = "Bearer";

async function getTokken(details) {
    const client = new MongoClient("mongodb://127.0.0.1:27017");
    try {
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Users');
        let res = await users.find({ username: details.username, password: details.password }).toArray();
        if (res.length > 0) {
            const data = { username: details.username }
            return { "status": 200, "tokken": jwt.sign(data, key) };
        } else {
            return { "status": 404 };
        }

    } catch {
        return { "status": 400 };
    } finally {
        client.close();
    }
}

function validateToken(beforeToken, token) {
    if (beforeToken !== smallBearer && beforeToken !== bigBearer) {
        return null;
    }
    try {
        const data = jwt.verify(token, key);
        return data;
    } catch {
        return null;
    }
}


export default { validateToken, getTokken }