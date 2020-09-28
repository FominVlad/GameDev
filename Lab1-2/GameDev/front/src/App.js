import React, {useState} from 'react';
import Menu from './Menu';
import Board from './Board';
import './App.css';
import End from './End';

function App() {
  const [status, setStatus] = useState('menu');
  const [board, setBoard] = useState(null);
  const [players, setPlayers] = useState(null);
  const [size, setSize] = useState(8);
  const [current, setCurrent] = useState(0);
  const [winner, setWinner] = useState(null);

  const renderScreen = status => {
    switch (status) {
      case 'menu':
        return <Menu setStatus={setStatus} setPlayers={setPlayers} setSize={setSize} setBoard={setBoard} setCurrent={setCurrent} />;
      case 'game':
        return <Board players={players} size={size} board={board} current={current} setBoard={setBoard} setCurrent={setCurrent} setStatus={setStatus} setWinner={setWinner} />;
      case 'end':
        return <End winner={winner} setStatus={setStatus} />
      default:
        break;
    }
  };

  return (
    <div className="App">
      { renderScreen(status) }
    </div>
  );
}

export default App;
