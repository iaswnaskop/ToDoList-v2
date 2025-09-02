import React, { useState } from "react";
import {Eye, EyeOff} from "lucide-react"


function LoginPage({onLogin, onGoToRegister}) {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try{
            const response = await fetch("http://localhost:9090/api/login", {
                method: "POST",
                headers: {"Content-Type" : "application/json"},
                body: JSON.stringify({email, password})
            });

            if(response.ok){
                const data = await response.json();
                

                localStorage.setItem("token", data.token);

                onLogin();
            }
            else{
                alert("Wrong credentials");
            }
        }
        catch(error){
            console.error("Login error:", error);

        }

    };


    return(
        <div className="bg-white shadow-lg rounded-3xl p-16 w-full max-w-md">
            <h1 className="text-3xl font-bold text-center text-gray-900 mb-6">
                Login
            </h1>

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <input 
                        type="text"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" 
                    />
                </div>

                <div className="relative w-full">
                     <input 
                        type={showPassword ? "text" : "password"}
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500" 
                    />
                    <button
                        type="button"
                        onClick={() => setShowPassword((prev) => !prev)}
                        className="absolute inset-y-0 right-10 flex items-center text-gray-500 hover:text-gray-700"
                    >
                        {setShowPassword ? <EyeOff size={18}/> : <Eye size={18}/>}
                    </button>
                </div>

                <button
                    type="sumbit"
                    className="w-full bg-blue-500 text-white px-4 py-2 rounded-lg hover:bg-blue-600"
                >
                    Login
                </button>
            </form>

            <p className="text-center mt-4 text-gray-600">
                Do you want to Register?{""}
                <button onClick={onGoToRegister} className="text-blue-500 hover:underline">
                    Register here
                </button>
            </p>
        </div>

    );
}

export default LoginPage;