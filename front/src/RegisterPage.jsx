import React, {useState, useEffect} from "react";


function RegisterPage({onBackToLogin, onRegisterSuccess}){
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [roles, setRoles] = useState([]);
    const [selectedRole, setSelectedRole] = useState("");

    useEffect(() => {
        const fetchRoles = async () => {
            try{
                const response = await fetch("http://localhost:9090/api/roles");
                const data = await response.json();
                console.log(data);
                setRoles(data);
            }
            catch(error){
                console.error("Error fetching roles:", error);
            }
        };
        fetchRoles();
    }, []);
 
    const handleSubmit = async (e) => {
        e.preventDefault();
        console.log("Send data:",selectedRole)
        console.log(typeof selectedRole);

        try{
            const response = await fetch("http://localhost:9090/api/register",{
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify({email, password, name, roleId: parseInt(selectedRole)})
            });
            
            
            if(response.ok){
                onRegisterSuccess();
            }
            else{
                const errorData = await response.json();
                alert(errorData.massage || "Registration failed");
            }

        }
        catch(error){
            console.error("Register error:", error);
        }
    };

    return(
        <div className="bg-white shadow-lg rounded-3xl p-16 w-full max-w-md">
            <h1 className="text-3xl font-bold text-center text-gray-900 mb-6">
                Register
            </h1>
            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <input
                        type="name"
                        placeholder="Username"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                </div>
                <div>
                    <input
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                </div>
                <div>
                    <input
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500"
                    />
                </div>

                <div>
                    <select value={selectedRole} onChange={(e) => setSelectedRole(e.target.value)} className="w-full px-3 py-2 border rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500">
                        <option value="">--Select Role--</option>
                            {roles.map((role) => (
                                <option key={role.id} value={role.id}>{role.name}</option>
                            ))}
                    </select>
                </div>
                <button type="sumbit" className="w-full bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600">
                    Register
                </button>
            </form>

            <p className="text-center mt-4"> Already have an acoount?
                <button onClick={onBackToLogin} className="text-blue-500 hover:underline">Login</button>
            </p>
        </div>

    );
}

export default RegisterPage;