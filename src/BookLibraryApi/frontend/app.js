const API = "http://localhost:5186/api";
let token = localStorage.getItem("token");

function updateAuthUI() {
  const loggedIn = !!token;
  document.getElementById("add-book-section").style.display = loggedIn ? "block" : "none";
  document.getElementById("logout-btn").style.display = loggedIn ? "inline" : "none";
  document.getElementById("login-btn").style.display = loggedIn ? "none" : "inline";
  document.getElementById("register-btn").style.display = loggedIn ? "none" : "inline";
}

async function loadBooks() {
  const res = await fetch(`${API}/books`);
  const books = await res.json();
  const list = document.getElementById("book-list");
  list.innerHTML = "";
  books.forEach(b => {
    const li = document.createElement("li");
    li.innerHTML = `<span><strong>${b.title}</strong> — ${b.authorName} (${b.publishedYear})</span>`;
    if (token) {
      const del = document.createElement("button");
      del.textContent = "Delete";
      del.onclick = () => deleteBook(b.id);
      li.appendChild(del);
    }
    list.appendChild(li);
  });
}

async function deleteBook(id) {
  await fetch(`${API}/books/${id}`, {
    method: "DELETE",
    headers: { Authorization: `Bearer ${token}` }
  });
  loadBooks();
}

document.getElementById("login-btn").onclick = async () => {
  const body = {
    username: document.getElementById("username").value,
    password: document.getElementById("password").value
  };
  const res = await fetch(`${API}/auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body)
  });
  if (!res.ok) return alert("Login failed");
  const data = await res.json();
  token = data.token; localStorage.setItem("token", token);
  updateAuthUI(); loadBooks();
};

document.getElementById("register-btn").onclick = async () => {
  const body = {
    username: document.getElementById("username").value,
    password: document.getElementById("password").value
  };
  const res = await fetch(`${API}/auth/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body)
  });
  if (!res.ok) return alert("Register failed");
  const data = await res.json();
  token = data.token; localStorage.setItem("token", token);
  updateAuthUI(); loadBooks();
};

document.getElementById("logout-btn").onclick = () => {
  token = null; localStorage.removeItem("token");
  updateAuthUI(); loadBooks();
};

document.getElementById("book-form").onsubmit = async e => {
  e.preventDefault();
  const body = {
    title: document.getElementById("title").value,
    isbn: document.getElementById("isbn").value || null,
    publishedYear: parseInt(document.getElementById("year").value),
    authorId: parseInt(document.getElementById("authorId").value)
  };
  const res = await fetch(`${API}/books`, {
    method: "POST",
    headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
    body: JSON.stringify(body)
  });
  if (!res.ok) return alert("Add failed");
  e.target.reset(); loadBooks();
};

updateAuthUI();
loadBooks();
