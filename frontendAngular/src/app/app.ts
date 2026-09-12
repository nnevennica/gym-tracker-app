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

  // forma workout
  exerciseTypeId = '11111111-1111-1111-1111-111111111111';
  dateTime = new Date().toISOString().slice(0, 16);
  durationMinutes = 45;
  caloriesBurned = 300;
  intensityRating = 7;
  fatigueRating = 5;
  notes = '';

  // filteri;tabela
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
}