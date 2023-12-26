"use client";

import React from "react";
import Countdown, { zeroPad } from "react-countdown";
import { RxLapTimer } from "react-icons/rx";

type Props = {
  auctionEnd: string;
};

const renderer = ({
  days,
  hours,
  minutes,
  seconds,
  completed,
}: {
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
  completed: boolean;
}) => {
  return (
    <div
      className={`
    border-1 border-white text-white py-1 px-2 rounded-lg flex justify-center
    ${
      completed
        ? "bg-red-600"
        : days === 0 && hours < 10
        ? "bg-amber-600"
        : "bg-gray-900"
    }`}
    >
      {completed ? (
        <span>Auciton Finished</span>
      ) : (
        <span suppressHydrationWarning={true} className="flex text-sm">
          <RxLapTimer size={12} className="mt-1 mr-1" /> {zeroPad(days)}:
          {zeroPad(hours)}:{zeroPad(minutes)}:{zeroPad(seconds)}
        </span>
      )}
    </div>
  );
};

export default function CountdownTimer({ auctionEnd }: Props) {
  return (
    <div>
      <Countdown date={auctionEnd} renderer={renderer}></Countdown>
    </div>
  );
}
