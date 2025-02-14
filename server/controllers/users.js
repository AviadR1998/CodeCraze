import myModels from '../models/users.js'

const addUser = async (req, res) => {
    const myRes = await myModels.insrtUser(req.body);
    if (myRes === 409) {
        res.status(409);
    } else {
        res.status(200).json(myRes);
    }
    res.end();
}

const returnUser = async (req, res) => {
    const myRes = await myModels.getUserInfo(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes === 401) {
        res.status(401);
    } else {
        res.status(200).json({ username: myRes.username, mail: myRes.mail, date: myRes.date });
    }
    res.end();
}

const resetUser = async (req, res) => {
    const myRes = await myModels.resetUserModels(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    res.status(myRes);
    res.end();
}

const deleteUser = async (req, res) => {
    const myRes = await myModels.delUser(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    res.status(myRes);
    res.end();
}

const addScore = async (req, res) => {
    const myRes = await myModels.updateScore(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1], req.body.score);
    res.status(myRes);
    res.end();
}

const topScore = async (req, res) => {
    const myRes = await myModels.getTopScore(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1]);
    if (myRes === 401) {
        res.status(401);
    } else {
        res.status(200).json({ players: myRes });
    }
    res.end();
}

const saveState = async (req, res) => {
    const myRes = await myModels.saveState(req.headers.authorization.split(" ")[0], req.headers.authorization.split(" ")[1], req.body);
    res.status(myRes);
    res.end();

}


const getState = async (req, res) => {
    try {
        const authHeader = req.headers.authorization || "";
        const [bearer, token] = authHeader.split(" ");

        const myRes = await myModels.getState(bearer, token);

        if (myRes === 401) {
            console.log("Unauthorized access");
            res.status(401).json({ error: "Unauthorized" });
        } else {
            res.status(200).json(myRes);
        }
    } catch (err) {
        console.error("Error in getState:", err);
        res.status(500).json({ error: "Server error" });
    }
};

export { addUser, returnUser, deleteUser, resetUser, addScore, topScore, saveState, getState };