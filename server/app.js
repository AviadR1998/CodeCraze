import express from 'express';
import bodyParser from 'body-parser';
import usersRouter from './routes/users.js';
import tokensRouter from './routes/tokens.js';
import questionsRouter from './routes/questions.js';
import roomsRouter from './routes/rooms.js';
import { roomsList, roomsListTopics } from "./models/rooms.js";
import cors from 'cors';
const app = express();

import http from 'http';
const server = http.createServer(app);
import { Server } from "socket.io";

var socketArr = new Map();
const io = new Server(3000);

export const myIo = io;
export const arrSoc = socketArr;
io.on('connection', (socket) => {
    socket.on('username', (username) => {
        socketArr.set(username, socket);
    })
    socket.on('disconnect', (username) => {
        if (roomsList.has(username)) {
            if (roomsList.get(username) != "--") {
                io.to(socket).emit('host', null);
            }
        }
        socketArr.delete(username);

    });

    socket.on('cord', (username, x, y, z, speed) => {
        socketArr.get(username).emit('cord', x, y, z, speed);
    });

    socket.on('finish', (username) => {
        roomsList.delete(username);
        roomsListTopics.delete(username);
    });

    socket.on('disjoin', (username) => {
        roomsList.set(username, "--");
        socketArr.get(username).emit("disjoin");
    });

});



app.use(cors());
app.use(bodyParser());
app.use('/api/Users', usersRouter);
app.use('/api/Tokens', tokensRouter);
app.use('/api/Questions', questionsRouter);
app.use('/api/Rooms', roomsRouter);
app.use((req, res, next) => {
    console.log(`Unhandled route: ${req.method} ${req.originalUrl}`);
    res.status(404).json({ error: "Not Found" });
});

app.use(express.static('public'));
app.listen(5000);