import { Component, Input, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Member } from '../../../models/member.model';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [
    MatCardModule, MatIconModule
  ],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.scss'
})
export class MemberCardComponent {
  @Input('memberIn') member: Member | undefined;
}
