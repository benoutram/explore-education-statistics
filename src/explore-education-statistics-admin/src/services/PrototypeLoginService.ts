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
    { id: 'user1', name: 'John Smith', permissions: [''] },
    { id: 'user2', name: 'Ann Evans', permissions: [''] },
    { id: 'user3', name: 'Stephen Doherty', permissions: [''] },
    { id: 'user4', name: 'User 4', permissions: [''] },
    { id: 'user5', name: 'User 5', permissions: [''] },
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

  public static getNoLoggedInUser() {
    return {};
  }
}
