import express from 'express'
import { generateTokken } from '../controllers/tokens.js'
var tokensRouter = express.Router();

tokensRouter.post('/', generateTokken);

export default tokensRouter;