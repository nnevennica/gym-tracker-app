import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WorkoutService, WeeklyStats, WorkoutPayload } from './services/workout.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  isRegisterMode = false;
  currentUserId: string | null = localStorage.getItem('userId');

  // forma auth
  email = '';
  password = '';
  firstName = '';
  lastName = '';

  // profil
  heightCm: number = Number(localStorage.getItem('heightCm')) || 172;
  weightKg: number = Number(localStorage.getItem('weightKg')) || 64;
  weeklyGoalWorkouts: number = Number(localStorage.getItem('weeklyGoalWorkouts')) || 4;

  // forma profila
  showProfileModal = false;
  editHeight = this.heightCm;
  editWeight = this.weightKg;
  editGoal = this.weeklyGoalWorkouts;

  // forma workout
  exerciseTypeId = '11111111-1111-1111-1111-111111111111';
  dateTime = new Date().toISOString().slice(0, 16);
  durationMinutes = 45;
  caloriesBurned = 300;
  intensityRating = 7;
  fatigueRating = 5;
  notes = '';

  //tabela i filteri
  filterYear = 2026;
  filterMonth = 9;
  weeklyStats: WeeklyStats[] = [];

  constructor(
    private workoutService: WorkoutService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    if (this.currentUserId) {
      this.loadProgress();
    }
  }

  toggleAuthMode(): void {
    this.isRegisterMode = !this.isRegisterMode;
  }

  handleAuth(): void {
    if (this.isRegisterMode) {
      this.authService.register({
        email: this.email,
        password: this.password,
        firstName: this.firstName,
        lastName: this.lastName
      }).subscribe({
        next: (res: { message: string; userId: string }) => {
          alert('Registracija uspešna!');
          this.currentUserId = res.userId;
          if (this.currentUserId) {
            localStorage.setItem('userId', this.currentUserId);
          }
          this.loadProgress();
        },
        error: (err: any) => alert(err.error?.error || 'Greška pri registraciji.')
      });
    } else {
      this.currentUserId = '00000000-0000-0000-0000-000000000001';
      localStorage.setItem('userId', this.currentUserId);
      this.loadProgress();
    }
  }

  logout(): void {
    localStorage.removeItem('userId');
    this.currentUserId = null;
  }

  openProfileModal(): void {
    this.editHeight = this.heightCm;
    this.editWeight = this.weightKg;
    this.editGoal = this.weeklyGoalWorkouts;
    this.showProfileModal = true;
  }

  closeProfileModal(): void {
    this.showProfileModal = false;
  }

  saveProfile(): void {
    this.heightCm = this.editHeight;
    this.weightKg = this.editWeight;
    this.weeklyGoalWorkouts = this.editGoal;

    localStorage.setItem('heightCm', this.heightCm.toString());
    localStorage.setItem('weightKg', this.weightKg.toString());
    localStorage.setItem('weeklyGoalWorkouts', this.weeklyGoalWorkouts.toString());

    this.showProfileModal = false;
  }

  saveWorkout(): void {
    if (!this.currentUserId) return;

    const payload: WorkoutPayload = {
      userId: this.currentUserId,
      exerciseTypeId: this.exerciseTypeId,
      dateTime: this.dateTime,
      durationMinutes: this.durationMinutes,
      caloriesBurned: this.caloriesBurned,
      intensityRating: this.intensityRating,
      fatigueRating: this.fatigueRating,
      notes: this.notes
    };

    this.workoutService.createWorkout(payload).subscribe({
      next: () => {
        alert('Trening uspešno sačuvan!');
        this.notes = '';
        this.loadProgress();
      },
      error: (err: any) => alert(err.error?.error || 'Greška pri čuvanju.')
    });
  }

  loadProgress(): void {
    if (!this.currentUserId) return;

    this.workoutService.getMonthlyProgress(this.currentUserId, this.filterYear, this.filterMonth).subscribe({
      next: (res) => this.weeklyStats = res.weeklyStats,
      error: () => this.weeklyStats = []
    });
  }

  getLatestWorkoutCount(): number {
    if (this.weeklyStats.length === 0) return 0;
    return this.weeklyStats[this.weeklyStats.length - 1].totalWorkouts;
  }

  getGoalPercentage(): number {
    const count = this.getLatestWorkoutCount();
    const pct = Math.round((count / this.weeklyGoalWorkouts) * 100);
    return pct > 100 ? 100 : pct;
  }

  getSmartInsight(): { text: string; alertType: string; status: string } {
    if (this.weeklyStats.length === 0) {
      return {
        text: 'Unesite prvi trening kako bi pametni asistent izračunao status oporavka.',
        alertType: 'alert-secondary',
        status: 'Čeka se unos'
      };
    }

    const latest = this.weeklyStats[this.weeklyStats.length - 1];
    if (latest.averageFatigue >= 7.5) {
      return {
        text: 'Prosečan umor ove nedelje je povišen. Preporučujemo lakši kardio ili dan odmora radi kvalitetnog oporavka.',
        alertType: 'alert-warning',
        status: 'Potreban odmor'
      };
    } else if (latest.averageIntensity >= 7) {
      return {
        text: 'Sjajna konzistentnost i visok intenzitet! Tvoj organizam se odlično adaptira na trenutno opterećenje.',
        alertType: 'alert-success',
        status: 'Optimalna forma'
      };
    } else {
      return {
        text: 'Stabilan ritam treninga. Imaš prostora za blago povećanje intenziteta ili trajanja naredne nedelje.',
        alertType: 'alert-info',
        status: 'Uravnoteženo'
      };
    }
  }
}