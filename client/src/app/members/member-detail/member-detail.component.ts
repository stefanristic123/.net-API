import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Message } from 'src/app/_models/message';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [CommonModule, TabsModule, GalleryModule, FormsModule]
})
export class MemberDetailComponent implements OnInit, OnDestroy  {
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  messages: Message[] = [];
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  activeTab?: TabDirective;
  messageForm: NgForm | undefined;
  messageContent = '';
  user?: User;

  constructor(
    private memberService: MembersService, 
    private route: ActivatedRoute, 
    public messageService: MessageService,
    public presenceService: PresenceService,
    private accountService: AccountService 
    ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if (user) this.user = user;
        }
      })
     }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get('username');
    if (!username) return;
    this.memberService.getMember(username).subscribe({
      next: member => {
        this.member = member,
        this.getImages()
        this.loadMessages(member.username)
      }
    })
  }

  getImages() {
    if (!this.member) return;
    for (const photo of this.member?.photos) {
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
    }
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.user) {
      this.messageService.createHubConnection(this.user, this.member.username);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  loadMessages(username: string) {
    this.messageService.getMessageThread(username).subscribe({
      next: messages => this.messages = messages
    })
  }

  sendMessage(username: string){
    // this.messageService.sendMessage(username, this.messageContent).subscribe({
    //   next: message => {
    //     this.messages.push(message);
    //     this.messageForm?.reset();
    //     this.messageContent = '';
    //   }
    // })

    this.messageService.sendMessage(username, this.messageContent).then(() => {
      this.messageForm?.reset();
      this.messageContent = '';
    })
  }

}
