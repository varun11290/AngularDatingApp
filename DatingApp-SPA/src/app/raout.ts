import { Routes } from '@angular/router'
import { HomeComponent } from './home/home.component'
import { ListComponent } from './list/list.component';
import { MembersListComponent } from './members-list/members-list.component';
import { MassageComponent } from './massage/massage.component';
import { AuthGuard } from './_guard/auth.guard';

export const routs: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'list', component: ListComponent },
            { path: 'members', component: MembersListComponent, canActivate: [AuthGuard] },
            { path: 'massage', component: MassageComponent }

        ]
    },
    { path: '**', redirectTo: '' }
];
