import myModels from '../models/tokens.js'

const generateTokken = async (req, res) => {
    const myRes = await myModels.getTokken(req.body);
    if (myRes.status === 200) {
        res.status(200).send(myRes.tokken);
    } else {
        res.status(myRes.status);
    }
    res.end();
}


export { generateTokken };