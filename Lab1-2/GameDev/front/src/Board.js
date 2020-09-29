import React, {useState, useEffect} from 'react'
import './Board.css';
import { URL, COLORS, GET_MOVES, MAKE_MOVE, TYPES } from './models/Api';
import { modifyBoard } from './util';

const Board = ({size, board, players, current, setBoard, setCurrent, setWinner, setStatus}) => {
    const styles = {
        gridTemplateColumns: 'repeat('+ size +', 1fr)',
        gridTemplateRows: 'repeat('+ size +', 1fr)',
    }
    
    const playerColors = Object.entries(COLORS).map(([k, v]) => ({id: players.find(p => p.playerColour === v).id, type: k, color: v}));
    
    const [moves, setMoves] = useState([]);
    const [skip, setSkip] = useState(false);
    
    useEffect(() => {
        if (!players.find(p => p.id === current)) {
            const ps = players.map(p => ({name: p.name, count: board.filter(c => c.ownerId === p.id).length})).sort((a, b) => b.count - a.count); 
            console.log(ps);
            setWinner(ps[0].name);
            setStatus('end');
        } else {
            if (players.find(p => p.id === current).playerType === TYPES.CPU) makeMove(0, 0, true)();
            else {
                fetch(URL + '?playerId=' + current, {
                    method: GET_MOVES,
                }).then(res => (res.status === 200) ? res.json() : null)
                .then(data => 
                    data
                    ? setMoves(data)
                    : null
                );
            }
        }
    }, [current, skip]);
    
    const makeMove = (x, y, isCpu = false) => e => {
        console.log({isCpu});
        const move = moves.find(m => m.posX === x && m.posY === y);
        if (!move && !isCpu) return;
        fetch(URL + '?playerId=' + current, {
            method: MAKE_MOVE,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({posX: x, posY: y}),
        }).then(res => (res.status === 200) ? res.json() : null)
        .then(data => 
            data
            ? (setBoard(modifyBoard(board, data.changedChips)), setCurrent(data.nextStepPlayerId), setSkip(data.nextStepPlayerId === current ? !skip : skip))
            : null
        );
    };
    

    return (
        <div className="card-body text-center">
            <h1 className="card-title">{(players.find(p => p.id === current) || {}).name}'s turn</h1>
            {
                players.map(p => <p>{p.name}: {board.filter(c => c.ownerId === p.id).length} points</p>)
            }
            <div className="board-wrapper">
                <div className="board-container">
                    <div className="board">
                        <div className="bg">
                            <BoardBG />
                        </div>
                        <div className="grid" style={styles}>
                            {
                                board.map((cell, i) => <Cell key={[cell.x, cell.y].join(',')} click={makeMove(cell.x, cell.y)} allowed={moves.some(m => m.posX === cell.x && m.posY === cell.y)} colors={playerColors} {...cell} />)
                            }
                        </div>
                    </div>
                </div>
            </div>
            <button className="btn btn-primary mt-4" onClick={() => setStatus('menu')}>Menu</button>
        </div>
    );
};

const BoardBG = () => (
    <svg className="img" xmlns="http://www.w3.org/2000/svg">
        <defs>
            <filter id="grain" x="0%" y="0%" width="100%" height="100%" filterUnits="objectBoundingBox" primitiveUnits="userSpaceOnUse" colorInterpolationFilters="linearRGB">
                <feTurbulence type="fractalNoise" baseFrequency="0.4 0.4" numOctaves="3" seed="0" x="0%" y="0%" width="100%" height="100%" result="turbulence2"/>
                <feBlend mode="multiply" x="0%" y="0%" width="100%" height="100%" in="SourceGraphic" in2="turbulence2" result="blend"/>
            </filter>
        </defs>
        <rect width="100%" height="100%" fill="green" filter="url(#grain)" />
    </svg>
);

const Cell = ({x, y, ownerId, click, colors, allowed}) => {
    const styles = {
        gridColumn: x+1,
        gridRow: y+1,
        background: allowed ? 'radial-gradient(red 6%, transparent 15%)' : 'none',
    };

    return (
        <div className="cell" style={styles} onClick={click}>
            <Chip owner={ownerId} colors={colors}/>
        </div>
    );
};

const Chip = ({owner, colors}) => {
    if (typeof owner !== 'undefined') {
        const type = colors.find(color => color.id === owner).type;

        return (
            <svg className="chip" viewBox="0 0 10 10">
                <defs>
                    <radialGradient id={type + 'grad'}>
                        <stop offset="60%" stopColor="white" stopOpacity="1" />
                        <stop offset="100%" stopColor="white" stopOpacity="0.8" />
                    </radialGradient>
                    <mask id={type + 'mask'}>
                        <circle cx="5" cy="5" r="5" fill={'url(#' + type + 'grad)'} />
                    </mask>
                </defs>
                <circle fill={type} mask={'url(#'+type+'mask)'} cx="5" cy="5" r="5"/>
            </svg>
        );
    } else {
        return null;
    }
};

export default Board;