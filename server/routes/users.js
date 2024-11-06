import express from 'express'
import { addUser, returnUser, deleteUser } from '../controllers/users.js'
var usersRouter = express.Router();

usersRouter.post('/', addUser);
usersRouter.get('/:user', returnUser);
usersRouter.delete('/delete', deleteUser);

export default usersRouter;