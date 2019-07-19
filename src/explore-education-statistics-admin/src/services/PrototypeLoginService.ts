export interface User {
  id: string;
  name: string;
  permissions: string[];
}

export interface Authentication {
  user?: User;
}

export class PrototypeLoginService {
  private static currentUser: User;

  private static USERS: User[] = [
    { id: 'user1', name: 'John Smith', permissions: ['team lead'] },
    { id: 'user2', name: 'Ann Evans', permissions: ['team member'] },
    {
      id: 'user3',
      name: 'Stephen Doherty',
      permissions: ['responsible statistician'],
    },
    { id: 'user4', name: 'User 4', permissions: ['team member'] },
    { id: 'user5', name: 'User 5', permissions: ['team member'] },
  ];

  public static getUserList(): User[] {
    return PrototypeLoginService.USERS;
  }

  public static getUser(userId: string) {
    return PrototypeLoginService.USERS.filter(user => user.id === userId)[0];
  }

  public static getAuthentication(userId: string) {
    return {
      user: PrototypeLoginService.getUser(userId),
    };
  }

  public static setActiveUser(userId: string) {
    window.localStorage.setItem('userId', userId);
  }

  public static login() {
    return PrototypeLoginService.getAuthentication(
      window.localStorage.getItem('userId') || 'John Smith',
    );
  }

  public static getNoLoggedInUser() {
    return {};
  }
}
