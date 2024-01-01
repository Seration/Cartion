import Image from "next/image";
import Listings from "./auctions/Listings";

export default function Home() {
  return (
    <div>
      {/* <div className="w-full bg-gray-200 aspect-w-16 aspect-h-6 rounded-lg overflow-hidden">
        <Image
          alt="banner"
          width={1000}
          height={200}
          src="https://plus.unsplash.com/premium_photo-1661940814738-5a028d647d3a?q=80&w=870&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
        ></Image>
      </div> */}

      <div className="mt-4 mb-2">
        <span className="font-semibold">
          Current Bids
        </span>
      </div>

      <Listings />
    </div>
  );
}
