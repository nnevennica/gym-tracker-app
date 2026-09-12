import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface WeeklyStats {
  weekNumber: number;
  totalWorkouts: number;
  totalDurationMinutes: number;
  averageIntensity: number;
  averageFatigue: number;
}

export interface MonthlyProgressResponse {
  userId: string;
  year: number;
  month: number;
  weeklyStats: WeeklyStats[];
}

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

export interface WorkoutItem {
  id: string;
  exerciseTypeName: string;
  dateTime: string;
  durationMinutes: number;
  caloriesBurned: number;
  intensityRating: number;
  fatigueRating: number;
  notes?: string;
}

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private readonly apiUrl = 'https://localhost:7096/api/workouts';

  constructor(private http: HttpClient) {}

  createWorkout(payload: WorkoutPayload): Observable<any> {
    return this.http.post(this.apiUrl, payload);
  }

  getMonthlyProgress(userId: string, year: number, month: number): Observable<MonthlyProgressResponse> {
    const params = new HttpParams()
      .set('userId', userId)
      .set('year', year.toString())
      .set('month', month.toString());

    // Vraćeno na staru rutu sa kojom radi backend controller
    return this.http.get<MonthlyProgressResponse>(`${this.apiUrl}/progress`, { params });
  }

  getWorkoutsForWeek(userId: string, year: number, month: number, weekNumber: number): Observable<WorkoutItem[]> {
    const params = new HttpParams()
      .set('userId', userId)
      .set('year', year.toString())
      .set('month', month.toString())
      .set('weekNumber', weekNumber.toString());

    return this.http.get<WorkoutItem[]>(`${this.apiUrl}/weekly-details`, { params });
  }
}