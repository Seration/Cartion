import React from "react";
import Search from "./Search";
import Logo from "./Logo";
import LoginButton from './LoginButton';
import { getCurrentUser } from '../actions/authActions';
import UserActions from './UserActions';

export default async function Navbar() {

    const user = await getCurrentUser();

  return (
    <header className="sticky z-10 top-0 z-550 flex justify-between bg-white p-5 items-center text-gray-800 shadow-md">
      <Logo></Logo>
      <Search></Search>
      {user ? (
        <UserActions user={user} />
      ) : (
        <LoginButton />
      )}
    </header>
  );
}
