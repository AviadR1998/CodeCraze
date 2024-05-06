import express from 'express'
import { addUser, returnUser } from '../controllers/users.js'
var usersRouter = express.Router();

usersRouter.post('/', addUser);
usersRouter.get('/:user', returnUser);

export default usersRouter;