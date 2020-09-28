import React from 'react';

const End = ({winner, setStatus}) => {

    return (
        <div>
            <h1>Winner: {winner}</h1>
            <button onClick={() => setStatus('menu')}>Menu</button>
        </div>
    );
};

export default End;