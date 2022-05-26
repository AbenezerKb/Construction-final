import React, { useState } from 'react';

function ToggleBox({ title, children }) {
  const [isOpened, setIsOpened] = useState(false);

  function toggle() {
    setIsOpened(wasOpened => !wasOpened);
  }

  return (
    <div className="box">
      <div className="boxTitle" onClick={toggle}>
        {title}
      </div>
      {isOpened && (
        <div className="boxContent">
          {children}
        </div>
      )}
    </div>
  );
}

export default ToggleBox;