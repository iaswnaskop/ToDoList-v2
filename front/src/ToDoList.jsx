import React, { useState, useEffect } from "react";


function ToDolist({onLogout}){
    const [todos,setTodos] = useState([]);
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [statusList, setStatusList] = useState([]);
    const [currentUser, setCurrentUSer] = useState("");

   const token = localStorage.getItem("token");
   const [editingTodoId, setEditingTodoId] = useState(null);
   const [editedTitle, setEditedTitle] = useState("");
   const [editedDescription, setEditedDescription] = useState("");
   const [editedStatusId, setEditedStatusId] = useState("");

    
    useEffect(() =>{
        const fetchTasks = async () => {
            try {
                
                const response = await fetch(`http://localhost:5098/api/getUserByToken`,{
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`
                    }
                });
                const data = await response.json();
                console.log("UserTasks Details Data:", data);
                localStorage.setItem("All Users", JSON.stringify(data.allUsers));
                localStorage.setItem("Current User", JSON.stringify(data.currentUser));
                localStorage.setItem("Status List", JSON.stringify(data.statusList));
                localStorage.setItem("Tasks", JSON.stringify(data.tasks));

                setTodos(data.tasks || []);
                setStatusList(data.statusList || []);
                setCurrentUSer(data.currentUser);
            } catch (error) {
                console.error("Error fetching user datails:", error);
            }
        };
        fetchTasks();
    }, []);
    

    const addToDo = () => {
        if(title.trim() && description.trim()) {
            
            
            const userId = currentUser.id
           // console.log(defaultStatusId);
            console.log(userId);
            console.log(token);
            const newTodo = {
                task: {
                    title: title,
                    description: description
                    
                },
                userId: [userId],
                
            };
            console.log(newTodo);

            fetch("http://localhost:5098/api/createTask", {
                method: "POST",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(newTodo),
            })
                .then((res) => res.json())
                .then((savedTodo) => {
                    setTodos([...todos, savedTodo]);
                    setTitle("");
                    setDescription("");
                })
                .catch((err) => console.error("Error adding todo:", err));
        }
    };

    const handleEditClick = (todo) => {
        setEditingTodoId(todo.id);
        setEditedTitle(todo.title);
        setEditedDescription(todo.description);
        setEditedStatusId(todo.statusId);
    };


    const handleCancleClick = () => {
        setEditingTodoId(null);
    };

    const handleSaveClick = (id) => {
        const updatedTask ={
            task: {
                    title: editedTitle,
                    description: editedDescription,
                    statusId: editedStatusId
                    
                },
                userId: [],
        }
        console.log("Updated Data", updatedTask);

        fetch(`http://localhost:5098/api/updateTask/${id}`, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json"
            },
            body: JSON.stringify(updatedTask)
        })
        .then(res => res.json())
        .then(savedUpdatedTodo => {
            setTodos(todos.map(t => (t.id === id ? savedUpdatedTodo : t)));
            setEditingTodoId(null);
        })
        .catch(err => console.error("Error updating task:", err));
    };
   


    const deleteTodo = (id) => {
        fetch(`http://localhost:5098/api/deleteTask/${id}`,{
            method: "DELETE",
             headers: {
                "Authorization": `Bearer ${token}`
            },
        })
            .then(() => {
                setTodos(todos.filter((t) => t.id !== id));
            })
            .catch((err) => console.error("Error deleting todo:", err));
    };

    const handleLogout = () => {
        localStorage.removeItem("token");
        localStorage.removeItem("Current User");
        localStorage.removeItem("All Users");
        localStorage.removeItem("Status List");
        localStorage.removeItem("Tasks");

        onLogout();

    }

    const groupedTodos = todos.reduce((groups, todo) =>{
        if(!groups[todo.statusId]){
            groups[todo.statusId] = [];
        }
        groups[todo.statusId].push(todo);
        return groups;
    },{});


    return(
        <div className="bg-white shadow-lg rounded-3xl p-16">
            <div className="flex justify-between items-center mb-6">
            <h1 className="text-3xl font-bold text-center text-gray-900">Welcome {currentUser.name}</h1>
            <button onClick={handleLogout} className="bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-700 transition-colors">Logout</button>
            </div>
            <h2 className="text-2xl font-bold text-center text-gray-800 mb-6">ToDo List</h2>

            <div className="mb-4 flex">
                <input value={title} onChange={(e) => setTitle(e.target.value)} type="text" placeholder="Add a new task" className="flex-grow px-3 py-2 border rounded-l-lg focus:outline-none focus:ring-2 focus:ring-blue-500" />
                <input value={description} onChange={(e) => setDescription(e.target.value)} type="text" placeholder="Add a description" className="flex-grow px-3 py-2 border rounded-r focus:outline-none focus:ring-2 focus:ring-blue-500"/>
                <button onClick={addToDo} className="bg-blue-500 text-white px-4 py-2 rounded-r-lg hover:bg-blue-600">Add</button>
            </div>


            <div className="space-y-6">
                {statusList.map((status) => (
                    <div key={status.id}>
                        <h2 className="text-lg font-bold mb-2 capitalize">{status.name}</h2>
                        {groupedTodos[status.id] && groupedTodos[status.id].length > 0 ? (
                            <ul className="space-y-2">
                                {groupedTodos[status.id].map((todo) => (
                                    <li key={todo.id} className="flex items-center p-3 rounded-lg bg-slate-100 border border-gray-200">
                                        {editingTodoId === todo.id ? (
                                            <>
                                            <div className="flex-grow space-y-2 mb-2">
                                                <input type="text" value={editedTitle} onChange={(e) => setEditedTitle(e.target.value)} className="w-full px-2 py-1 border rounded"></input>
                                                <input type="text" value={editedDescription} onChange={(e) => setEditedDescription(e.target.value)} className="w-full px-2 py-1 border rounded"></input>
                                                <select value={editedStatusId} onChange={(e) => setEditedStatusId(e.target.value)} className="w-full px-2 py-1 border rounded">
                                                    {statusList.map(status => (<option key={status.id} value={status.id}>{status.name}</option>))}
                                                </select>
                                            </div>
                                            <div className="flex-shrink-0 ml-4 space-x-2">
                                                <button onClick={() => handleSaveClick(todo.id)} className="border-none p-2 rounded-lg bg-green-500 text-white hover:bg-green-600">Save</button>
                                                <button onClick={handleCancleClick} className="border-none p-2 rounded-lg bg-gray-500 text-white hover:bg-gray-600">Cancel</button>
                                            </div>
                                            </>
                                        ) : (
                                            <>
                                                <div className="flex-grow">
                                                    <p className="font-bold text-gray-800">{todo.title}</p>
                                                    <p className="text-sm text-gray-600">{todo.description}</p>
                                                </div>
                                                <div className="flex-shrink-0 ml-4 space-x-2">
                                                    <button onClick={() => handleEditClick(todo)} className="border-none p-2 rounded-lg bg-yellow-500 text0white hover:bg-yellow-600">Edit</button>
                                                    <button onClick={() => deleteTodo(todo.id)} className="border-none p-2 rounded-lg bg-red-500 text-white hover:bg-red-600">Delete</button>
                                                </div>
                                            </>
                                        )}
                                        
                                    </li>
                                ))}
                            </ul>
                        ): (
                            <p className="text-gray-500">No Tasks in this Status</p>
                        )}
                    </div>
                ))}
            </div>
        </div>
    );
}

export default ToDolist