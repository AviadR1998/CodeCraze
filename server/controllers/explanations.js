import myModels from '../models/explanations.js'

const addExplanations = async (req, res) => {
    const myRes = await myModels.postexplanations(req.body);
    res.status(myRes)
    res.end();
}

const returnExplanationsByTopic = async (req, res) => {
    const myRes = await myModels.getQuestionsByTopic(req.params.topic, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes === 401) {
        res.status(401);
    } else {
        res.status(200).send(myRes);
    }
    res.end();
}

export { returnExplanationsByTopic, addExplanations };