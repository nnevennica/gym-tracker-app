import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface WorkoutPayload {
  userId: string;
  exerciseTypeId: string;
  dateTime: string;
  durationMinutes: number;
  caloriesBurned: number;
  intensityRating: number;
  fatigueRating: number;
  notes?: string;
}

export interface WeeklyStats {
  weekNumber: number;
  startOfWeek: string;
  endOfWeek: string;
  totalWorkouts: number;
  totalDurationMinutes: number;
  averageIntensity: number;
  averageFatigue: number;
}

export interface MonthlyProgress {
  year: number;
  month: number;
  weeklyStats: WeeklyStats[];
}

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private readonly apiUrl = 'https://localhost:7096/api/workouts';

  constructor(private http: HttpClient) {}

  createWorkout(payload: WorkoutPayload): Observable<{ id: string }> {
    return this.http.post<{ id: string }>(this.apiUrl, payload);
  }

  getMonthlyProgress(userId: string, year: number, month: number): Observable<MonthlyProgress> {
    return this.http.get<MonthlyProgress>(`${this.apiUrl}/progress?userId=${userId}&year=${year}&month=${month}`);
  }
}