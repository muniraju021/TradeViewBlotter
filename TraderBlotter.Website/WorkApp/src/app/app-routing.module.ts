import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginComponent} from './components/public/login/login.component';
import { HomeComponent } from './components/secure/home/home.component';
import { AuthGuard } from './shared/common/auth.guard'
import {UserComponent} from './components/secure/user/user.component'
import { ClientDealerMappingComponent } from './components/secure/client-dealer-mapping/client-dealer-mapping.component';
import { ManageUserComponent } from './components/secure/manage-user/manage-user.component';
import {NetPositionViewComponent} from './components/secure/net-position-view/net-position-view.component';

const routes: Routes = [
    { path: 'home', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'user', component: UserComponent },
    { path: 'client-dealer-mapping', component: ClientDealerMappingComponent },
    { path: 'manage-user', component: ManageUserComponent },
    { path: 'net-position-view', component: NetPositionViewComponent },
    // otherwise redirect to home
    { path: '**', redirectTo: 'home' }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
