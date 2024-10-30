import { MongoClient } from "mongodb";
import functions from "./tokens.js"

const key = "my secret key";

async function postQuestion(details) {
    const client = new MongoClient("mongodb://127.0.0.1:27017");
    try {
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Questions');
        const ret = { question: details.question, options: details.options, answer: details.answer, tag: details.tag, level: details.level};
        await users.insertOne(ret);
        return ret;
    } catch{
        let res;
        res.status = 401;
        return res;
    } finally {
        client.close();
    }
}

async function getQuestionsByLevel(level, bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const questions = db.collection('Questions');
        let res = await questions.find({ level: level }).toArray();
        return res;
    } catch (err) {
        return 401;
        //return res.status(401).send("Invalid Token");
    } finally {
        //client.close();
    }
}

async function getQuestionsByTag(tag, bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const questions = db.collection('Questions');
        let res = await questions.find({ tag: tag }).toArray();
        return res;
    } catch (err) {
        return 401;
        //return res.status(401).send("Invalid Token");
    } finally {
        //client.close();
    }
}

export default { getQuestionsByLevel, getQuestionsByTag, postQuestion }