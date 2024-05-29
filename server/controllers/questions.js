import myModels from '../models/questions.js'

const returnQuestionsByLevel = async (req, res) => {
    const myRes = await myModels.getQuestionsByLevel(req.params.level, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes.status === 200) {
        res.status(200).send(myRes.tokken);
    } else {
        res.status(myRes.status);
    }
    res.end();
}

const returnQuestionsByTag = async (req, res) => {
    const myRes = await myModels.getQuestionsByTag(req.params.tag, req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes.status === 200) {
        res.status(200).send(myRes.tokken);
    } else {
        res.status(myRes.status);
    }
    res.end();
}

export { returnQuestionsByLevel, returnQuestionsByTag };