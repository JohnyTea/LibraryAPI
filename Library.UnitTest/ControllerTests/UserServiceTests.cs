namespace Library.UnitTest.ControllerTests;

public class UserServiceTests : TestBase
{
    [Fact]
    public async void GetAll_Should_ReturnAllUsers()
    {

        var service = new UserService(_context);

        var result = await service.GetAllAsync();

        Assert.Equal(_context.Users.Count(), result.Count());

    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async void GetByID_Should_ReturnOneUser(int ID)
    {
        var correctBook = _context.Users.First(user => user.Id == ID);
        var service = new UserService(_context);

        var result = await service.GetByIdAsync(ID);

        Assert.NotNull(result);
        Assert.Equal(result.Id, ID);
        Assert.Equal(result.UserName, correctBook.UserName);
        Assert.Equal(result.BirthDate, correctBook.BirthDate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2345)]
    [InlineData(-1263)]
    [InlineData(-1)]
    public async Task GetByID_Should_ThrowElementNotFoundException(int ID)
    {
        var service = new UserService(_context);

        await Assert.ThrowsAsync<ElementNotFoundException>(() => service.GetByIdAsync(ID));
    }

    [Theory]
    [InlineData("Łukasz Cydejko", "1999-01-11")]
    public async void Add_Should_AddNewUser(string userName, string dateSTR)
    {
        string[] dateSplitted = dateSTR.Split('-');
        DateTime date = new(int.Parse(dateSplitted[0]), int.Parse(dateSplitted[1]), int.Parse(dateSplitted[2]));
        UserService service = new(_context);
        int userCountBeforePost = _context.Users.Count();
        User newUser = new()
        {
            UserName = userName,
            BirthDate = date
        };

        await service.AddAsync(newUser);

        int userCountAfterPost = _context.Users.Count();

        Assert.Equal(userCountBeforePost + 1, userCountAfterPost);
        var addedUser = _context.Users.FirstOrDefault(user =>
                user.UserName == userName &&
                user.BirthDate == date
        );
        Assert.NotNull(addedUser);
    }

    [Fact]
    public async void Add_Should_AddExistingUser()
    {
        User firstUser = _context.Users.First();
        User bookToPost = new()
        {
            UserName = firstUser.UserName,
            BirthDate = firstUser.BirthDate
        };
        UserService service = new(_context);
        int usersCountBeforePost = _context.Users.Count();


        await service.AddAsync(bookToPost);


        int usersCountAfterPost = _context.Users.Count();

        Assert.Equal(usersCountBeforePost + 1, usersCountAfterPost);
        var addedUser = _context.Users.Where(user =>
                user.UserName == firstUser.UserName &&
                user.BirthDate == firstUser.BirthDate
        );
        Assert.NotNull(addedUser);
        Assert.Equal(2, addedUser.Count());
    }

    [Theory]
    [InlineData(2, "Łukasz", "2022-02-01")]
    public async void Edit_Should_UpdateUser(int id, string userName, string dateSTR)
    {
        string[] dateSplitted = dateSTR.Split('-');
        DateTime date = new(int.Parse(dateSplitted[0]), int.Parse(dateSplitted[1]), int.Parse(dateSplitted[2]));
        UserService service = new(_context);
        int usersCountBeforePut = _context.Users.Count();
        User updatedUser = new()
        {
            Id= id,
            UserName = userName,
            BirthDate = date
        };

        await service.EditAsync(id, updatedUser);

        int usersCountAfterPut = _context.Users.Count();

        Assert.Equal(usersCountBeforePut, usersCountAfterPut);
        var changedUser = _context.Users.Where(user =>
               user.UserName == userName &&
               user.BirthDate == date
        );
        Assert.NotNull(changedUser);
        Assert.Equal(1, changedUser.Count());
        Assert.Equal(updatedUser.BirthDate, changedUser.First().BirthDate);
        Assert.Equal(updatedUser.UserName, changedUser.First().UserName);
    }

    [Theory]
    [InlineData(2)]
    public async void Delete_Should_DeleteUser(int id)
    {
        int oldUsersCount = _context.Users.Count();
        UserService service = new(_context);

        await service.DeleteByIdAsync(id);

        int newUsersCount = _context.Users.Count();
        Assert.Equal(oldUsersCount - 1, newUsersCount);
        var deletedUsers = _context.Users.FirstOrDefault((user) => user.Id == id);
        Assert.Null(deletedUsers);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(132)]
    public async void Delete_Should_DoNothing(int id)
    {
        int oldUsersCount = _context.Users.Count();
        BookService service = new(_context);

        await service.DeleteByIdAsync(id);

        int newUsersCount = _context.Users.Count();
        Assert.Equal(oldUsersCount, newUsersCount);

        var deletedUsers = _context.Users.FirstOrDefault((user) => user.Id == id);
        Assert.Null(deletedUsers);
    }

}

