const API_URL = "https://localhost:7096/api";

let isRegisterMode = false;
let currentUserId = localStorage.getItem("userId") || null;

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("workoutDate").value = new Date().toISOString().slice(0, 16);
    if (currentUserId) {
        showDashboard();
    }
});

function toggleAuthMode() {
    isRegisterMode = !isRegisterMode;
    document.getElementById("authTitle").innerText = isRegisterMode ? "Registracija" : "Prijava";
    document.getElementById("registerFields").classList.toggle("d-none", !isRegisterMode);
    document.getElementById("authSubmitBtn").innerText = isRegisterMode ? "Registruj se" : "Prijavi se";
    document.getElementById("toggleAuthBtn").innerText = isRegisterMode ? "Već imate nalog? Prijavite se" : "Nemate nalog? Registrujte se";
}

async function handleAuth(event) {
    event.preventDefault();
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    if (isRegisterMode) {
        const firstName = document.getElementById("firstName").value;
        const lastName = document.getElementById("lastName").value;

        const response = await fetch(`${API_URL}/auth/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password, firstName, lastName })
        });

        const data = await response.json();
        if (response.ok) {
            alert("Registracija uspešna! Poslat je email dobrodošlice.");
            currentUserId = data.userId;
            localStorage.setItem("userId", currentUserId);
            showDashboard();
        } else {
            alert(data.error || "Greška pri registraciji.");
        }
    } else {
        currentUserId = "00000000-0000-0000-0000-000000000001";
        localStorage.setItem("userId", currentUserId);
        showDashboard();
    }
}

function showDashboard() {
    document.getElementById("authSection").classList.add("d-none");
    document.getElementById("dashboardSection").classList.remove("d-none");
    document.getElementById("logoutBtn").classList.remove("d-none");
    loadProgress();
}

function logout() {
    localStorage.removeItem("userId");
    location.reload();
}

async function saveWorkout(event) {
    event.preventDefault();

    const payload = {
        userId: currentUserId,
        exerciseTypeId: document.getElementById("exerciseType").value,
        dateTime: document.getElementById("workoutDate").value,
        durationMinutes: parseInt(document.getElementById("duration").value),
        caloriesBurned: parseInt(document.getElementById("calories").value),
        intensityRating: parseInt(document.getElementById("intensity").value),
        fatigueRating: parseInt(document.getElementById("fatigue").value),
        notes: document.getElementById("notes").value
    };

    const response = await fetch(`${API_URL}/workouts`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload)
    });

    if (response.ok) {
        alert("Trening uspešno sačuvan!");
        document.getElementById("workoutForm").reset();
        document.getElementById("workoutDate").value = new Date().toISOString().slice(0, 16);
        loadProgress();
    } else {
        const data = await response.json();
        alert(data.error || "Greška pri čuvanju treninga.");
    }
}

async function loadProgress() {
    const year = document.getElementById("filterYear").value;
    const month = document.getElementById("filterMonth").value;

    const response = await fetch(`${API_URL}/workouts/progress?userId=${currentUserId}&year=${year}&month=${month}`);
    const tableBody = document.getElementById("progressTableBody");

    if (response.ok) {
        const data = await response.json();
        tableBody.innerHTML = "";

        if (data.weeklyStats.length === 0) {
            tableBody.innerHTML = `<tr><td colspan="5" class="text-center text-muted">Nema unetih treninga za izabrani mesec.</td></tr>`;
            return;
        }

        data.weeklyStats.forEach(stat => {
            const row = `
                <tr>
                    <td><strong>Nedelja ${stat.weekNumber}</strong></td>
                    <td><span class="badge bg-primary">${stat.totalWorkouts}</span></td>
                    <td>${stat.totalDurationMinutes} min</td>
                    <td><span class="badge bg-warning text-dark">${stat.averageIntensity} / 10</span></td>
                    <td><span class="badge bg-info text-dark">${stat.averageFatigue} / 10</span></td>
                </tr>
            `;
            tableBody.innerHTML += row;
        });
    }
}