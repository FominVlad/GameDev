import React from 'react';

const End = ({winner, setStatus}) => {
    return (
        <div className="card-body text-center">
            <h1 className="card-title">Winner: {winner}</h1>
            <button className="btn btn-primary" onClick={() => setStatus('menu')}>Menu</button>
        </div>
    );
};

export default End;