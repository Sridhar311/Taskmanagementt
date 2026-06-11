"use client";

import { useEffect, useState } from "react";
import { api } from "@/services/api";
import ProtectedRoute from "@/components/ProtectedRoute";

export default function Dashboard() {
  const [tasks, setTasks] = useState<any[]>([]);

  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [status, setStatus] = useState("Pending");
  const [priority, setPriority] = useState("Medium");
  const [dueDate, setDueDate] = useState("");

  const [editingTaskId, setEditingTaskId] = useState<number | null>(null);

  const [search, setSearch] = useState("");
  const [filterStatus, setFilterStatus] = useState("");

  const [page, setPage] = useState(1);
  const [pageSize] = useState(5);

  const [totalCount, setTotalCount] = useState(0);

  const totalPages = Math.ceil(totalCount / pageSize);

  const resetForm = () => {
    setEditingTaskId(null);
    setTitle("");
    setDescription("");
    setStatus("Pending");
    setPriority("Medium");
    setDueDate("");
  };

  const fetchTasks = async () => {
    try {
      const res = await api.get(
        `/tasks?page=${page}&pageSize=${pageSize}&search=${search}&status=${filterStatus}`
      );

      const responseData = res.data.data || res.data;

      setTasks(responseData.items);
      setTotalCount(responseData.totalCount);
    } catch (error) {
      console.error(error);
      alert("Failed to fetch tasks");
    }
  };

  const createTask = async () => {
    try {
      await api.post("/tasks", {
        title,
        description,
        status,
        priority,
        dueDate,
      });

      resetForm();
      await fetchTasks();
    } catch (error) {
      console.error(error);
      alert("Failed to create task");
    }
  };

  const updateTask = async () => {
    if (editingTaskId === null) return;

    try {
      await api.patch(`/tasks/${editingTaskId}`, {
        title,
        description,
        status,
        priority,
        dueDate,
      });

      resetForm();
      await fetchTasks();
    } catch (error) {
      console.error(error);
      alert("Failed to update task");
    }
  };

  const deleteTask = async (id: number) => {
    const confirmed = window.confirm(
      "Are you sure you want to delete this task?"
    );

    if (!confirmed) return;

    try {
      await api.delete(`/tasks/${id}`);
      await fetchTasks();
    } catch (error) {
      console.error(error);
      alert("Failed to delete task");
    }
  };

  const editTask = (task: any) => {
    setEditingTaskId(task.id);
    setTitle(task.title);
    setDescription(task.description);
    setStatus(task.status);
    setPriority(task.priority);
    setDueDate(task.dueDate?.split("T")[0] || "");
  };

  useEffect(() => {
    fetchTasks();
  }, [page, search, filterStatus]);

  return (
    <ProtectedRoute>
      <div className="p-6 max-w-4xl mx-auto">
        {/* Create/Edit Task */}
        <div className="border p-4 rounded mb-6">
          <h2 className="text-xl font-semibold mb-4">
            {editingTaskId ? "Edit Task" : "Create Task"}
          </h2>

          <input
            className="border p-2 w-full mb-2"
            placeholder="Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
          />

          <textarea
            className="border p-2 w-full mb-2"
            placeholder="Description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />

          <select
            className="border p-2 w-full mb-2"
            value={status}
            onChange={(e) => setStatus(e.target.value)}
          >
            <option value="Pending">Pending</option>
            <option value="InProgress">In Progress</option>
            <option value="Completed">Completed</option>
          </select>

          <select
            className="border p-2 w-full mb-2"
            value={priority}
            onChange={(e) => setPriority(e.target.value)}
          >
            <option value="Low">Low</option>
            <option value="Medium">Medium</option>
            <option value="High">High</option>
          </select>

          <input
            type="date"
            className="border p-2 w-full mb-4"
            value={dueDate}
            onChange={(e) => setDueDate(e.target.value)}
          />

          <button
            onClick={editingTaskId ? updateTask : createTask}
            className="bg-green-500 text-white px-4 py-2 rounded"
          >
            {editingTaskId ? "Update Task" : "Create Task"}
          </button>

          {editingTaskId && (
            <button
              onClick={resetForm}
              className="ml-2 bg-gray-500 text-white px-4 py-2 rounded"
            >
              Cancel
            </button>
          )}
        </div>

        {/* Search & Filter */}
        <div className="border p-4 rounded mb-6">
          <h2 className="text-xl font-semibold mb-4">
            Search & Filter
          </h2>

          <input
            className="border p-2 w-full mb-2"
            placeholder="Search tasks..."
            value={search}
            onChange={(e) => {
              setSearch(e.target.value);
              setPage(1);
            }}
          />

          <select
            className="border p-2 w-full mb-2"
            value={filterStatus}
            onChange={(e) => {
              setFilterStatus(e.target.value);
              setPage(1);
            }}
          >
            <option value="">All Statuses</option>
            <option value="Pending">Pending</option>
            <option value="InProgress">In Progress</option>
            <option value="Completed">Completed</option>
          </select>
        </div>

        {/* Task List */}
        <h1 className="text-2xl font-bold mb-4">
          My Tasks
        </h1>

        {tasks.length === 0 ? (
          <p>No tasks found.</p>
        ) : (
          tasks.map((task) => (
            <div
              key={task.id}
              className="border p-4 rounded mb-3"
            >
              <h2 className="text-lg font-semibold">
                {task.title}
              </h2>

              <p className="text-gray-600">
                {task.description}
              </p>

              <p>Status: {task.status}</p>
              <p>Priority: {task.priority}</p>

              {task.dueDate && (
                <p>
                  Due Date:{" "}
                  {new Date(task.dueDate).toLocaleDateString()}
                </p>
              )}

              <div className="mt-3">
                <button
                  onClick={() => editTask(task)}
                  className="bg-yellow-500 text-white px-3 py-1 rounded mr-2"
                >
                  Edit
                </button>

                <button
                  onClick={() => deleteTask(task.id)}
                  className="bg-red-500 text-white px-3 py-1 rounded"
                >
                  Delete
                </button>
              </div>
            </div>
          ))
        )}

        {/* Pagination */}
        <div className="flex items-center gap-4 mt-6">
          <button
            onClick={() =>
              setPage((p) => Math.max(1, p - 1))
            }
            disabled={page === 1}
            className="bg-gray-500 text-white px-4 py-2 rounded disabled:opacity-50"
          >
            Prev
          </button>

          <span>
            Page {page} of {totalPages || 1}
          </span>

          <button
            onClick={() => setPage((p) => p + 1)}
            disabled={page >= totalPages}
            className="bg-blue-500 text-white px-4 py-2 rounded disabled:opacity-50"
          >
            Next
          </button>
        </div>
      </div>
    </ProtectedRoute>
  );
}