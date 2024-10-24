import myModels from '../models/questions.js'

const addQuestion = async (req, res) => {
    const myRes = await myModels.postQuestion(req.params);
    if (myRes === 200) {
        res.status(200).send(myRes);
    } else {
        res.status(401);
    }
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

const returnQuestionsByTag = async (req, res) => {
    const myRes = await myModels.getQuestionsByTag(req.params.tag, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes.status === 401) {
        res.status(401);
    } else {
        res.status(200).send(myRes);
    }
    res.end();
}

export { returnQuestionsByLevel, returnQuestionsByTag, addQuestion };