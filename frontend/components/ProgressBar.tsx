import React from "react";
import * as CSS from "csstype";

type ProgressBarProps = {
  bgcolor: string;
  progress: number;
  height: number;
};

const ProgressBar = ({ bgcolor, progress, height }: ProgressBarProps) => {
  const Parentdiv = {
    height: height,
    width: "100%",
    // backgroundColor: "whitesmoke",
    borderRadius: 40,
  };

  const Childdiv = {
    height: "100%",
    width: `${progress}%`,
    backgroundColor: bgcolor,
    borderRadius: 40,
    textAlign: "right",
  };

  const progresstext = {
    padding: 10,
    color: "black",
    fontWeight: 900,
  };

  return (
    <div
      style={{
        display: "flex",
        height: "100vh",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        padding: 10,
      }}
    >
      <div style={Parentdiv}>
        <div style={Childdiv as CSS.Properties<string | number>}>
          <span style={progresstext}>{`${progress}%`}</span>
        </div>
      </div>
    </div>
  );
};

export default ProgressBar;
