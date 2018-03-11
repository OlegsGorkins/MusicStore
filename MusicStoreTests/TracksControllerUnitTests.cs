using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MusicStore.Models;
using MusicStore.Controllers;
using MusicStore.Data;
using Xunit;
using FluentAssertions;

namespace MusicStoreTests
{
    public class TracksControllerUnitTests
    {
        [Fact]
        public async void Details_Values()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                //Arrange
                var options = new DbContextOptionsBuilder<MusicStoreContext>()
                    .UseSqlite(connection)
                    .Options;

                var context = new MusicStoreContext(options);
                context.Database.EnsureCreated();
                SeedData.SeedDatabase(context);

                var controller = new TracksController(context);

                int id = 1;

                //Act
                var result = await controller.Details(id);

                //Assert
                var view = result.Should().BeOfType<ViewResult>().Subject;
                var track = view.Model.Should().BeAssignableTo<Track>().Subject;
                track.ID.Should().Be(id);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
