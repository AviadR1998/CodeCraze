import { MongoClient } from "mongodb";
import functions from "./tokens.js"

const key = "my secret key";

async function postExplanations(details) {
    const client = new MongoClient("mongodb://127.0.0.1:27017");
    try {
        client.connect();
        const db = client.db('CodeCraze');
        const users = db.collection('Explanations');
        const line = { question: details.question, options: details.options, answer: details.answer, explanation: details.explanation, topic: details.topic};
        await users.insertOne(line);
        return 200;
    } catch{
        return 401;
    } finally {
        //client.close();
    }
}

async function getExplanationsByTopic(topic, bearer, token) {
    try {
        const data = functions.validateToken(bearer, token);
        if (data === null) {
            return 401;
        }
        const client = new MongoClient("mongodb://127.0.0.1:27017");
        client.connect();
        const db = client.db('CodeCraze');
        const questions = db.collection('Explanations');
        let res = await questions.find({ topic: topic }).toArray();
        return {questionList: res};
    } catch (err) {
        return 401;
        //return res.status(401).send("Invalid Token");
    } finally {
        //client.close();
    }
}

export default { getExplanationsByTopic, postExplanations }