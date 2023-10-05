using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            /**/
            
        }
        public class DataFetcher
        {
            private readonly HttpClient _httpClient;

            public DataFetcher()
            {
                _httpClient = new HttpClient();
                _httpClient.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");
            }

            public async Task<List<PostData>> GetPostsAsync()
            {
                var response = await _httpClient.GetAsync("posts");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    var posts = JsonSerializer.Deserialize<List<PostData>>(jsonString, options);
                    return posts;
                }
                else
                {
                    MessageBox.Show("Failed to retrieve data. Status code: " + response.StatusCode);
                    return null;
                }
            }
        }
        private async  void button1_Click(object sender, EventArgs e)
        {
            DataFetcher dataFetcher = new DataFetcher();
            var posts = await dataFetcher.GetPostsAsync();

            if (posts != null)
            {
                foreach (var post in posts)
                {
                    // Do something with the retrieved posts
                    MessageBox.Show($"Post #{post.UserId}: {post.Title}");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var postData = new PostData
            {
                Title = "John Doe",
                UserId = 30,
                Body  = "123 Main St"
            };

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

            var json = JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync("posts", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var responseClient = response.Content.ReadAsStringAsync().Result;
                var postResponse = JsonSerializer.Deserialize<PostResponse>(responseClient, options);
                MessageBox.Show("id - " + postResponse.Id);
                MessageBox.Show("id - " + postResponse.Name);
            }
            else
            {

            }
        }
    }
}
   
