import express from 'express'
import { returnAllRooms, createRoom, removeRoom, joinRoom, startGame } from '../controllers/rooms.js'
var roomsRouter = express.Router();

roomsRouter.post('/Create/:username', createRoom);
roomsRouter.post('/Join/:username', joinRoom);
roomsRouter.post('/Start/:username', startGame);
roomsRouter.delete('/Delete/:username', removeRoom)
roomsRouter.get('/:topics', returnAllRooms);

export default roomsRouter;