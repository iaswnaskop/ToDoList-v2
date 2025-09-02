import { use, useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import './ToDoList' 
import ToDolist from './ToDoList'
import LoginPage from './LoginPage'
import RegisterPage from './RegisterPage'

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isRegistering, setIsRegistering] = useState(false);

  const handleLogout = () => {
    setIsLoggedIn(false);
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-r from-blue-600 to emerald-400">
     {!isLoggedIn ? (
      isRegistering ? (
        <RegisterPage
          onRegisterSuccess={() => setIsRegistering(false)}
          onBackToLogin={() => setIsRegistering(false)}
        />
      ): (
        <LoginPage
          onLogin={() => setIsLoggedIn(true)}
          onGoToRegister={() => setIsRegistering(true)}
        />
      )
     ) : (<ToDolist onLogout={handleLogout}/>)}
    </div>
  );
}

export default App
