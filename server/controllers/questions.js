import myModels from '../models/questions.js'

const addQuestion = async (req, res) => {
    const myRes = await myModels.postQuestion(req.body);
    res.status(myRes)
    res.end();
}

const returnQuestionsByLevel = async (req, res) => {
    const myRes = await myModels.getQuestionsByLevel(req.params.level, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes.status === 401) {
        res.status(401);
    } else {
        res.status(200).send(myRes);
    }
    res.end();
}

const returnQuestionsByTopic = async (req, res) => {
    const myRes = await myModels.getQuestionsByTopic(req.params.topic, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes === 401) {
        res.status(401);
    } else {
        res.status(200).send(myRes);
    }
    res.end();
}

export { returnQuestionsByLevel, returnQuestionsByTopic, addQuestion };