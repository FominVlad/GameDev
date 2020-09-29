import React, {useState, useEffect} from 'react'
import {URL, START_GAME, COLORS, TYPES} from './models/Api';
import { createBoard } from './util';
// import './Menu.css';

const size = 8;

const Menu = ({setPlayers, setStatus, setSize, setBoard, setCurrent}) => {
    const [dark, setDark] = useState(false);
    const [error, setError] = useState(null);
    const [player1, setPlayer1] = useState({
        name: 'player1',
        playerType: TYPES.PLAYER,
    });
    const [player2, setPlayer2] = useState({
        name: 'player2',
        playerType: TYPES.CPU,
    });

    useEffect(() => {
        const setter = {
            true: 'add',
            false: 'remove',
        };

        document.body.classList[setter[dark]]('dark');
    })

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
        <div className="card-body">
            <h1 className="card-title">Menu</h1>
            <div className="center">
                {error ? <div className="error">{error}</div> : null}
                <form onSubmit={startGame}>
                    <div className="form-group row">
                        <label for="p1name" class="col-sm-4 col-form-label">Player 1 name</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="p1name" value={player1.name} onChange={nameOnChange(player1, setPlayer1)}/>
                        </div>
                    </div>
                    <div className="form-group row">
                        <label for="p2name" class="col-sm-4 col-form-label">Player 2 name</label>
                        <div class="col-sm-8">
                            <input type="text" class="form-control" id="p2name" value={player2.name} onChange={nameOnChange(player2, setPlayer2)}/>
                        </div>
                    </div>
                    <div className="form-group row">
                        <label for="isCpu" class="col-sm-4 col-form-label">Player 2 is CPU</label>
                        <div class="col-sm-8 col-form-label">
                            <input type="checkbox" id="isCpu" checked={player2.playerType === TYPES.CPU} onChange={isCpuOnChange(player2, setPlayer2)}/>
                        </div>
                    </div>
                    <div className="form-group row">
                        <label for="isDark" class="col-sm-4 col-form-label">Dark theme</label>
                        <div class="col-sm-8 col-form-label">
                            <input type="checkbox" id="isDark" checked={dark} onChange={e => setDark(e.target.checked)}/>
                        </div>
                    </div>
                    <input type="submit" class="btn btn-primary" value="Start"/>
                </form>
            </div>
        </div>
    );
};

export default Menu;