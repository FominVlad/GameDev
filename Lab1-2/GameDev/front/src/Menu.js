import React, {useState} from 'react'
import {URL, START_GAME, COLORS, TYPES} from './models/Api';
import { createBoard } from './util';
// import './Menu.css';

const size = 8;

const Menu = ({setPlayers, setStatus, setSize, setBoard, setCurrent}) => {
    const [error, setError] = useState(null);
    const [player1, setPlayer1] = useState({
        name: 'player1',
        playerType: TYPES.PLAYER,
    });
    const [player2, setPlayer2] = useState({
        name: 'player2',
        playerType: TYPES.CPU,
    });

    const startGame = e => {
        e.preventDefault();
        if (player1.name === player2.name) {
            setError('Player names should be different');
            return;
        }
        setError(null);

        const first = Math.round(Math.random());
        const players = [player1, player2].map((player, i) => 
            Object.assign({}, player, {playerColour: (i === first) ? COLORS.BLACK : COLORS.WHITE})
        );
        fetch(URL + '?boardSize=' + size, {
            method: START_GAME,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(players),
        }).then(res => (res.status === 201) ? res.json() : null)
        .then(data => 
            data
            ? (setPlayers(data.players), setCurrent(data.nextStepPlayerId), setBoard(createBoard(size, data.board)), setSize(size), setStatus('game'))
            : setError('The data you provided is not complete. Please check your info and try again')
        );
    }

    const nameOnChange = (obj, setter) => e => setter(Object.assign({}, obj, {name: e.target.value}));
    const isCpuOnChange = (obj, setter) => e => setter(Object.assign({}, obj, {playerType: e.target.checked ? 0 : 1}));

    return (
        <div className="menu">
            <div className="center">
                {error ? <div className="error">{error}</div> : null}
                <form onSubmit={startGame}>
                    <input type="text" value={player1.name} onChange={nameOnChange(player1, setPlayer1)}/>
                    <input type="text" value={player2.name} onChange={nameOnChange(player2, setPlayer2)}/>
                    <input type="checkbox" checked={player2.playerType === TYPES.CPU} onChange={isCpuOnChange(player2, setPlayer2)}/>
                    <input type="submit" value="Start"/>
                </form>
            </div>
        </div>
    );
};

export default Menu;