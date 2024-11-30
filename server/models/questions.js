import { MongoClient } from "mongodb";
import functions from "./tokens.js"

const key = "my secret key";

async function postQuestion(details) {
    const client = new MongoClient("mongodb://127.0.0.1:27017");
    try {
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Questions');
        const line = { question: details.question, options: details.options, answer: details.answer, explanation: details.explanation, topic: details.topic};
        await users.insertOne(line);
        return 200;
    } catch{
        return 401;
    } finally {
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
    } finally {
    }
}

async function getQuestionsByTopic(topic, bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const questions = db.collection('Questions');
        let res = await questions.find({ topic: topic }).toArray();
        return {questionList: res};
    } catch (err) {
        return 401;
    } finally {
    }
}

export default { getQuestionsByLevel, getQuestionsByTopic, postQuestion }